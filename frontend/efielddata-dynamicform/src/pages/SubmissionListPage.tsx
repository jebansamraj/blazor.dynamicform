import { Button, Container, MenuItem, Stack, TextField, Typography } from '@mui/material';
import { DataGrid, GridColDef } from '@mui/x-data-grid';
import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { api } from '../api/client';

export default function SubmissionListPage() {
  const { id } = useParams(); const navigate = useNavigate();
  const [rows, setRows] = useState<any[]>([]); const [status, setStatus] = useState('');
  const load = () => api.get(`/forms/${id}/submissions`, { params: { page: 1, pageSize: 20, status: status || undefined } }).then(r => setRows(r.data));
  useEffect(load, [id]);
  const cols: GridColDef[] = [{ field: 'id', headerName: 'Id' }, { field: 'submittedAt', headerName: 'Submitted At', flex: 1 }, { field: 'status', headerName: 'Status' }, { field: 'actions', headerName: 'Actions', renderCell: p => <Button onClick={() => navigate(`/submissions/${p.row.id}`)}>View</Button> }];
  return <Container sx={{ py: 2 }}><Typography variant='h4'>Submissions</Typography><Stack direction='row' spacing={1} my={2}><TextField type='date' /><TextField select value={status} label='Status' onChange={e=>setStatus(e.target.value)} sx={{ minWidth: 180 }}><MenuItem value=''>All</MenuItem><MenuItem value='Submitted'>Submitted</MenuItem></TextField><Button onClick={load}>Filter</Button></Stack><div style={{ width:'100%', overflow:'auto' }}><DataGrid rows={rows} columns={cols} autoHeight pageSizeOptions={[20]} /></div></Container>;
}
