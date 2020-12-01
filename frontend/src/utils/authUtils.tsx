
const saveToken = (token: string) => {
  localStorage.setItem('token', token)
}

const getToken = () => {
  return localStorage.getItem('token');
}

const saveUser = (user: object) => {
  localStorage.setItem('user', JSON.stringify(user));
}

const getUser = () => {
  return JSON.parse(localStorage.getItem('user') ?? "");
}

const getUserPermission = () => {
  const temp: string = getUser() || '{}'; 
  if (Object.keys(temp).length !== 0) {
    return JSON.parse(temp).permission;
  }
  return null;
}

const logout = () => {
  localStorage.clear();
}

export {
  saveToken,
  getToken,
  saveUser,
  getUser,
  logout,
  getUserPermission
}