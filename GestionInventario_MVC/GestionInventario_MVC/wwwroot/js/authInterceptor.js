// Interceptor para todas las llamadas API
const originalFetch = window.fetch;
window.fetch = async (url, options = {}) => {
    // Obtener token de localStorage o sessionStorage
    const token = localStorage.getItem('jwtToken') || sessionStorage.getItem('jwtToken');

    if (token && !url.includes('/api/auth/')) {
        options.headers = {
            ...options.headers,
            'Authorization': `Bearer ${token}`
        };
    }

    return originalFetch(url, options);
};

// Verificar autenticación al cargar la página
document.addEventListener('DOMContentLoaded', () => {
    const token = localStorage.getItem('jwtToken') || sessionStorage.getItem('jwtToken');
    if (token && window.location.pathname === '/Account/Login') {
        window.location.href = '/';
    }
});