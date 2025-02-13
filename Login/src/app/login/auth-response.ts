export interface AuthResponse {
    exito: number;
    mensaje: string;
    data?: {
      usuario: string;
      token: string;
    };
  }
  