import Cookies from 'js-cookie';
import data from './appsettings.json';

export const getExpires = () => {
    return data['expires'] || 7;
}

export const save = (jwt) => {
    Cookies.set('jwtToken', jwt, { expiresIn: `${getExpires()}`, patch: '' });
}
export const getToken = () => Cookies.get('jwtToken');
export const remove = () => Cookies.remove('jwtToken');
