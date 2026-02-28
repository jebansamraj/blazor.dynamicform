import { createSlice, PayloadAction } from '@reduxjs/toolkit';

interface AuthState { token: string | null; role: string | null; tenantId: number | null; }
const initialState: AuthState = { token: localStorage.getItem('token'), role: localStorage.getItem('role'), tenantId: Number(localStorage.getItem('tenantId')) || null };
const authSlice = createSlice({
  name: 'auth', initialState,
  reducers: {
    setAuth: (state, action: PayloadAction<AuthState>) => { Object.assign(state, action.payload); localStorage.setItem('token', action.payload.token ?? ''); localStorage.setItem('role', action.payload.role ?? ''); localStorage.setItem('tenantId', String(action.payload.tenantId ?? '')); },
    logout: (state) => { state.token = null; state.role = null; state.tenantId = null; localStorage.clear(); }
  }
});
export const { setAuth, logout } = authSlice.actions;
export default authSlice.reducer;
