
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
  return localStorage.getItem('user');
}

export {
  saveToken,
  getToken,
  saveUser,
  getUser
}