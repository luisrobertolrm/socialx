export interface GoogleUser {
  id: string;
  email: string;
  name: string;
  givenName: string;
  familyName: string;
  photo: string;
}

export interface GoogleAuthResponse {
  type: 'success' | 'cancel' | 'error';
  user?: GoogleUser;
  idToken?: string;
  accessToken?: string;
  error?: string;
}

export interface AuthState {
  isAuthenticated: boolean;
  user: GoogleUser | null;
  loading: boolean;
  error: string | null;
}
