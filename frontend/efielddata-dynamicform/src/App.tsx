import { Navigate, Route, Routes } from 'react-router-dom';
import LoginPage from './pages/LoginPage';
import DashboardPage from './pages/DashboardPage';
import FormBuilderPage from './pages/FormBuilderPage';
import FormRenderPage from './pages/FormRenderPage';
import SubmissionListPage from './pages/SubmissionListPage';
import SubmissionDetailPage from './pages/SubmissionDetailPage';
import ProtectedRoute from './components/ProtectedRoute';

export default function App() {
  return <Routes>
    <Route path='/login' element={<LoginPage />} />
    <Route path='/' element={<ProtectedRoute><DashboardPage /></ProtectedRoute>} />
    <Route path='/builder/:id?' element={<ProtectedRoute><FormBuilderPage /></ProtectedRoute>} />
    <Route path='/forms/:id' element={<ProtectedRoute><FormRenderPage /></ProtectedRoute>} />
    <Route path='/forms/:id/submissions' element={<ProtectedRoute><SubmissionListPage /></ProtectedRoute>} />
    <Route path='/submissions/:id' element={<ProtectedRoute><SubmissionDetailPage /></ProtectedRoute>} />
    <Route path='*' element={<Navigate to='/' />} />
  </Routes>;
}
