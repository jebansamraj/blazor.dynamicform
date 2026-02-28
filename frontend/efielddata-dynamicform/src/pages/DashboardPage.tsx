import { Button, Card, CardActions, CardContent, Container, Grid2, Stack, Typography } from '@mui/material';
import { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { api } from '../api/client';
import { RootState } from '../app/store';
import { logout } from '../slices/authSlice';
import { setForms } from '../slices/formSlice';

export default function DashboardPage() {
  const forms = useSelector((s: RootState) => s.forms.items); const dispatch = useDispatch(); const navigate = useNavigate();
  useEffect(() => { api.get('/forms').then(r => dispatch(setForms(r.data))); }, [dispatch]);
  const remove = async (id:number) => { await api.delete(`/forms/${id}`); dispatch(setForms(forms.filter(f => f.id !== id))); };
  return <Container sx={{ py: 3 }}><Stack direction='row' justifyContent='space-between' mb={2}><Typography variant='h4'>Dashboard</Typography><Stack direction='row' spacing={1}><Button onClick={() => navigate('/builder')} variant='contained'>Create Form</Button><Button onClick={() => {dispatch(logout()); navigate('/login');}}>Logout</Button></Stack></Stack><Grid2 container spacing={2}>{forms.map(form => <Grid2 size={{ xs: 12, md: 4 }} key={form.id}><Card><CardContent><Typography variant='h6'>{form.name}</Typography></CardContent><CardActions><Button onClick={()=>navigate(`/builder/${form.id}`)}>Edit</Button><Button onClick={()=>navigate(`/forms/${form.id}`)}>View</Button><Button color='error' onClick={()=>remove(form.id)}>Delete</Button></CardActions></Card></Grid2>)}</Grid2></Container>;
}
