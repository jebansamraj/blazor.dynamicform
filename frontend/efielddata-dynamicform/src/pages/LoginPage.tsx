import { Button, Container, Paper, Stack, TextField, Typography } from '@mui/material';
import { useState } from 'react';
import { useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { api } from '../api/client';
import { setAuth } from '../slices/authSlice';

export default function LoginPage() {
  const [username, setUsername] = useState('admin@tenant1.com');
  const [password, setPassword] = useState('Admin@123');
  const dispatch = useDispatch(); const navigate = useNavigate();
  const submit = async () => {
    const { data } = await api.post('/auth/login', { username, password });
    dispatch(setAuth({ token: data.token, role: data.role, tenantId: data.tenantId }));
    navigate('/');
  };
  return <Container maxWidth='sm' sx={{ mt: 10 }}><Paper sx={{ p: 3 }}><Stack spacing={2}><Typography variant='h5'>eFieldData.DynamicForm Login</Typography><TextField value={username} onChange={e => setUsername(e.target.value)} label='Username' /><TextField value={password} onChange={e => setPassword(e.target.value)} type='password' label='Password' /><Button variant='contained' onClick={submit}>Login</Button></Stack></Paper></Container>;
}
