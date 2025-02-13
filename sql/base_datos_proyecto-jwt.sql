USE [master]
GO
 
CREATE DATABASE [TestCujilema1]
GO 

USE [TestCujilema1]
GO 

CREATE SEQUENCE [dbo].[SecuenciaCodigoVerificacion] 
 AS [int]
 START WITH 100
 INCREMENT BY 1
 MINVALUE -2147483648
 MAXVALUE 2147483647
 CACHE 
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Personas](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Nombres] [nvarchar](64) NOT NULL,
	[Apellidos] [nvarchar](64) NOT NULL,
	[NumeroIdentificacion] [nvarchar](16) NOT NULL,
	[Email] [nvarchar](64) NOT NULL,
	[TipoIdentificacionID] [int] NOT NULL,
	[FechaCreacion] [datetime] NULL,
	[NumeroIdentificacionConcatenado]  AS (([NumeroIdentificacion]+'-')+CONVERT([nvarchar](16),[TipoIdentificacionID])) PERSISTED,
	[NombreCompleto]  AS (([Nombres]+' ')+[Apellidos]) PERSISTED NOT NULL,
	[Eliminado] [bit] NOT NULL,
	[CodigoVerificacion] [int] NULL,
	[EsVerificada] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_NumeroIdentificacion] UNIQUE NONCLUSTERED 
(
	[NumeroIdentificacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoIdentificacion]    Script Date: 2025-02-08 7:41:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoIdentificacion](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [varchar](64) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuario]    Script Date: 2025-02-08 7:41:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuario](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[PersonaID] [bigint] NOT NULL,
	[Usuario] [nvarchar](32) NOT NULL,
	[Password] [nvarchar](128) NOT NULL,
	[FechaCreacion] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Usuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_Usuario_Usuario] UNIQUE NONCLUSTERED 
(
	[Usuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Personas] ADD  DEFAULT (getdate()) FOR [FechaCreacion]
GO
ALTER TABLE [dbo].[Personas] ADD  CONSTRAINT [DF_Personas_Eliminado]  DEFAULT ((0)) FOR [Eliminado]
GO
ALTER TABLE [dbo].[Personas] ADD  CONSTRAINT [DF_Personas_EsVerificada]  DEFAULT ((0)) FOR [EsVerificada]
GO
ALTER TABLE [dbo].[Usuario] ADD  DEFAULT (getdate()) FOR [FechaCreacion]
GO
ALTER TABLE [dbo].[Personas]  WITH CHECK ADD  CONSTRAINT [FK_Personas_TipoIdentificacion] FOREIGN KEY([TipoIdentificacionID])
REFERENCES [dbo].[TipoIdentificacion] ([ID])
GO
ALTER TABLE [dbo].[Personas] CHECK CONSTRAINT [FK_Personas_TipoIdentificacion]
GO
ALTER TABLE [dbo].[Usuario]  WITH CHECK ADD  CONSTRAINT [FK_Usuario_Personas] FOREIGN KEY([PersonaID])
REFERENCES [dbo].[Personas] ([ID])
GO
ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [FK_Usuario_Personas]
GO
/****** Object:  StoredProcedure [dbo].[ActualizarPersona]    Script Date: 2025-02-08 7:41:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


---------------------------------------------------------------------------*****
----------- Stored procedures ---------------------------------------------
---------- Tipo de Identificación -----------------------------------------
---------------------------------------------------------------------------
 
-- =============================================
-- Author:		<Adela Lorena Cujilema>
-- Create date: <04-02-2025>
-- Description:	<PersonaActualizar>
-- =============================================
CREATE PROCEDURE [dbo].[ActualizarPersona]	
@Nombres as [nvarchar](64),
@Apellidos as [nvarchar](64),
@NumeroIdentificacion as [nvarchar](16),
@Email as [nvarchar](64),
@TipoIdentificacionID as int,
@FechaCreacion as datetime,
@Id as bigint

AS
BEGIN
	
	SET NOCOUNT ON;
	BEGIN TRY
		BEGIN TRANSACTION
		UPDATE Personas 
        SET 
            Nombres = @Nombres,
            Apellidos = @Apellidos,
            NumeroIdentificacion = @NumeroIdentificacion,
            Email = @Email,
            TipoIdentificacionID = @TipoIdentificacionID,
            FechaCreacion = @FechaCreacion
			
        where ID =  @Id 
		--AND Eliminado = 0

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
	END CATCH
    
END
GO
/****** Object:  StoredProcedure [dbo].[ConsultarPersonaPorCodigoVerificacion]    Script Date: 2025-02-08 7:41:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Lorena Cujilema>
-- Create date: <04-02-2025>
-- Description:	<ConsultarPersonaPorCodigoVerificacion>
-- =============================================
 --personaID, codigoVerificacion
CREATE PROCEDURE [dbo].[ConsultarPersonaPorCodigoVerificacion]
@personaID as int,
@codigoVerificacion as int
AS
BEGIN
    SELECT 
        --p.Id,
        p.Nombres,
        p.Apellidos,
        p.NumeroIdentificacion,
        p.Email,
        t.Descripcion as TipoIdentificacion,		
        p.FechaCreacion,
        p.NumeroIdentificacionConcatenado,
        p.NombreCompleto,
		p.CodigoVerificacion,
		p.Eliminado
    FROM Personas as p
	inner join TipoIdentificacion as t on p.TipoIdentificacionID = t.ID
	where p.ID = @personaID and p.CodigoVerificacion = @codigoVerificacion
	and p.Eliminado = 0
END;
GO
/****** Object:  StoredProcedure [dbo].[ConsultarPersonaPorID]    Script Date: 2025-02-08 7:41:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 -- =============================================
-- Author:		<Lorena Cujilema>
-- Create date: <04-02-2025>
-- Description:	<ConsultarPersonaPorID>
-- =============================================
CREATE PROCEDURE [dbo].[ConsultarPersonaPorID]
@ID as int
AS
BEGIN
    SELECT 
        --p.Id,
        p.Nombres,
        p.Apellidos,
        p.NumeroIdentificacion,
        p.Email,
        t.Descripcion as TipoIdentificacion,		
        p.FechaCreacion,
        p.NumeroIdentificacionConcatenado,
        p.NombreCompleto
    FROM Personas as p
	inner join TipoIdentificacion as t on p.TipoIdentificacionID = t.ID
	where p.ID = @ID
	and p.Eliminado = 0
END;
GO
/****** Object:  StoredProcedure [dbo].[ConsultarPersonaPorNumeroIdentificacion]    Script Date: 2025-02-08 7:41:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 -- ============================================= 
-- Author:		<Lorena Cujilema>
-- Create date: <04-02-2025>
-- Description:	<ConsultarPersonaPorNumeroIdentificacion>
-- =============================================
CREATE PROCEDURE [dbo].[ConsultarPersonaPorNumeroIdentificacion]
@NumeroIdentificacion as nvarchar(16)
AS
BEGIN
    SELECT 
        p.Id,
        p.Nombres,
        p.Apellidos,
        p.NumeroIdentificacion,
        p.Email,
        t.Descripcion as TipoIdentificacion,		
        p.FechaCreacion,
        p.NumeroIdentificacionConcatenado,
        p.NombreCompleto,
		p.TipoIdentificacionID,
		case p.Eliminado 
			when 1 then 'Inactivo'
			when 0 then 'Activo'
		end  as eliminado 
    FROM Personas as p
	inner join TipoIdentificacion as t on p.TipoIdentificacionID = t.ID
	where p.NumeroIdentificacion = @NumeroIdentificacion
	--and p.Eliminado = 0
END;
GO
/****** Object:  StoredProcedure [dbo].[ConsultarPersonas]    Script Date: 2025-02-08 7:41:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Lorena Cujilema>
-- Create date: <04-02-2025>
-- Description:	<Consultar General de Personas>
-- =============================================
CREATE PROCEDURE [dbo].[ConsultarPersonas]
AS
BEGIN
   SELECT 
        --p.Id,
		p.Nombres,
		p.Apellidos,
        p.NumeroIdentificacion,        
		t.Descripcion as TipoIdentificacion,        
        p.Email,
		case p.Eliminado 
			when 1 then 'Inactivo'
			when 0 then 'Activo'
		end  as eliminado       
    FROM Personas as p
	inner join TipoIdentificacion as t on p.TipoIdentificacionID = t.ID
	/* where p.Eliminado = 0 */

END;
GO
/****** Object:  StoredProcedure [dbo].[ConsultarUsuarioPorPersona]    Script Date: 2025-02-08 7:41:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 -- =============================================
-- Author:		<Lorena Cujilema>
-- Create date: <04-02-2025>
-- Description:	<ConsultarUsuarioPorPersona>
-- =============================================
CREATE PROCEDURE [dbo].[ConsultarUsuarioPorPersona]
@personaID as int
AS
BEGIN
    SELECT 
        PersonaID, Usuario, Password, FechaCreacion
	from Usuario
	where PersonaID = @personaID
END;
GO
/****** Object:  StoredProcedure [dbo].[GrabaPersona]    Script Date: 2025-02-08 7:41:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Adela Lorena Cujilema>
-- Create date: <04-02-2025>
-- Description:	<Grabar una nueva persona>
-- =============================================
CREATE PROCEDURE [dbo].[GrabaPersona]	
@Nombres	nvarchar(128),
@Apellidos	nvarchar(128),
@NumeroIdentificacion	nvarchar(32),
@Email	nvarchar(128),
@TipoIdentificacionID	int,
@codigoSecuencia as int OUTPUT
AS
BEGIN
	
	SET NOCOUNT ON;
	BEGIN TRY
		BEGIN TRANSACTION

		--Generar código verificación
		SET @codigoSecuencia = NEXT VALUE FOR SecuenciaCodigoVerificacion;

		INSERT INTO Personas (Nombres,Apellidos,NumeroIdentificacion,Email,TipoIdentificacionID, CodigoVerificacion)
		values (@Nombres,@Apellidos,@NumeroIdentificacion,@Email,@TipoIdentificacionID, @codigoSecuencia)

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
	END CATCH
    
END
GO
/****** Object:  StoredProcedure [dbo].[GrabarUsuario]    Script Date: 2025-02-08 7:41:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
-- =============================================
-- Author:		<Adela Lorena Cujilema>
-- Create date: <04-02-2025>
-- Description:	<Grabar un nuevo usuario>
-- =============================================
CREATE PROCEDURE [dbo].[GrabarUsuario]	
@PersonaID	bigint,
@Usuario	nvarchar(64),
@Password	nvarchar(256)
AS
BEGIN
	
	SET NOCOUNT ON;
	BEGIN TRY
		BEGIN TRANSACTION
 
		--Grabar usuario
		INSERT INTO Usuario (PersonaID,Usuario,Password)
		values (@PersonaID,@Usuario,@Password)

		--Actualizar estado EsVerificada en Persona
		UPDATE Personas 
		SET EsVerificada = 1
		where ID = @PersonaID

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
	END CATCH
    
END
GO
/****** Object:  StoredProcedure [dbo].[ObtenerPersonaID]    Script Date: 2025-02-08 7:41:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Lorena Cujilema>
-- Create date: <04-02-2025>
-- Description:	<obtener el personaID>
-- =============================================
CREATE PROCEDURE [dbo].[ObtenerPersonaID]
@NumeroIdentificacion varchar(16),
@CodigoVerificacion int

AS
BEGIN
	
	SET NOCOUNT ON;

	select p.ID as personaID 
	from Personas as p	
	where p.NumeroIdentificacion = @NumeroIdentificacion
	and p.CodigoVerificacion = @CodigoVerificacion
END
GO
/****** Object:  StoredProcedure [dbo].[ObtenerUsuarioPorClave]    Script Date: 2025-02-08 7:41:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Lorena Cujilema>
-- Create date: <04-02-2025>
-- Description:	<obtener username de usuario existente>
-- =============================================
CREATE PROCEDURE [dbo].[ObtenerUsuarioPorClave]
@Usuario nvarchar(32),
@Password nvarchar(128)
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT u.ID, u.PersonaID, u.Usuario as UserName, u.Password, p.Email
	FROM Usuario as u
	left join Personas as p on u.PersonaID = p.ID
	WHERE Usuario = @Usuario AND [Password] = @Password

END
GO
/****** Object:  StoredProcedure [dbo].[PersonaEliminadoLogico]    Script Date: 2025-02-08 7:41:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Adela Lorena Cujilema>
-- Create date: <04-02-2025>
-- Description:	<PersonaEliminadoLogico>
-- =============================================
create PROCEDURE [dbo].[PersonaEliminadoLogico]	
@Id	bigint
 
AS
BEGIN
	
	SET NOCOUNT ON;
	BEGIN TRY
		BEGIN TRANSACTION
 
		UPDATE Personas set Eliminado = 1 where ID =  @Id

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
	END CATCH
    
END
GO

---------------------------------------------------------------------------*****
----------- Inserci0n de data en ------------------------------------------
---------- Tipo de Identificaci0n -----------------------------------------
---------------------------------------------------------------------------
SET IDENTITY_INSERT [dbo].[TipoIdentificacion] ON 
GO
INSERT [dbo].[TipoIdentificacion] ([ID], [Descripcion]) VALUES (1, N'Cédula de Identidad')
GO
INSERT [dbo].[TipoIdentificacion] ([ID], [Descripcion]) VALUES (2, N'Pasaporte')
GO
INSERT [dbo].[TipoIdentificacion] ([ID], [Descripcion]) VALUES (3, N'RUC')
GO
SET IDENTITY_INSERT [dbo].[TipoIdentificacion] OFF
GO


---------------------------------------------------------------------------*****
----------- Inserci0n de data en ------------------------------------------
---------- Tipo de Identificaci0n -----------------------------------------
---------------------------------------------------------------------------




USE [master]
GO
ALTER DATABASE [TestCujilema1] SET  READ_WRITE 
GO
