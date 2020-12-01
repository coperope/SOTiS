
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
  let string = localStorage.getItem('user');
  if (string){
    return JSON.parse(string);
  }
  return null;
}

const getUserPermission = () => {
  const user = getUser(); 
  if (user) {
    return user.permission;
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