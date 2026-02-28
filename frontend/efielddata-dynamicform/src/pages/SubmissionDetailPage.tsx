import { Container, List, ListItem, Typography } from '@mui/material';
import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { api } from '../api/client';

export default function SubmissionDetailPage() {
  const { id } = useParams(); const [data, setData] = useState<any>();
  useEffect(() => { api.get(`/submissions/${id}`).then(r => setData(r.data)); }, [id]);
  return <Container sx={{ py: 2 }}><Typography variant='h4'>Submission #{id}</Typography><List>{Object.entries(data?.values ?? {}).map(([k,v]) => <ListItem key={k}>{k}: {String(v)}</ListItem>)}</List></Container>;
}
