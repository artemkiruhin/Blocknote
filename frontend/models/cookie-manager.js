import Cookies from 'js-cookie';
import data from './appsettings.json';

const getExpires = () => {
    return data['expires'] || 7;
}

const save = (jwt) => {
    Cookies.set('jwtToken', jwt, { expiresIn: `${getExpires()}`, patch: '' });
}
const getToken = () => Cookies.get('jwtToken');
const remove = () => Cookies.remove('jwtToken');
