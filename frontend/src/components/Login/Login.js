import React, { useState } from 'react';
import { Link } from 'react-router-dom'
import './Login.css';
import { BASE_URL, LOGIN } from '../../utils/apiUrls';
import { saveUser, saveToken } from '../../utils/authUtils';
import { useHistory } from 'react-router';

function Login() {
  const history = useHistory();
  const [usernameAlertHidden, setUsernameAlertHidden] = useState(true);
  const [passwordAlertHidden, setPasswordAlertHidden] = useState(true);
  const [password, setPassword] = useState('');
  const [username, setUsername] = useState('');
  //const [submited, setSubmited] = useState(false);

  const onChange = (e) => {
    if ('username' === e.target.name) {
      setUsernameAlertHidden(formValidation(e.target.value), setUsername(e.target.value))
    } else if ('password' === e.target.name) {
      setPasswordAlertHidden(formValidation(e.target.value), setPassword(e.target.value))
    }
  }
  const formValidation = (value, callback) => {
    if (value.length < 3) {
      return false;
    }
    if (callback) {
      callback();
    }
    return true;
  }
  const onSubmit = (e) => {
    e.preventDefault();
    if (formValidation(username)) {
      formValidation(password, () => {
        console.log('sending request...')
        if (usernameAlertHidden && passwordAlertHidden) {
          const requestOptions = {
            method: 'POST',
            headers: {
              Accept: 'application/json, text/plain, */*',
              'Content-Type': 'application/json;charset=UTF-8'
            },
            body: JSON.stringify({
              Username: username,
              Password: password
            })
          };
          const url = process.env.NODE_ENV === 'production' ? LOGIN : BASE_URL + LOGIN;
          fetch(url, requestOptions)
            .then((response) => {
              if (!response.ok) {
                alert("Invalid username or password")
              }
              else {
                alert("Successfuly logged in");
                return response.json();
              }
            })
            .then((data) => {
              if (data?.username) {
                const {token, ...user} = data;
                saveToken(token);
                saveUser(user);
                if (user.permission === 0){
                  history.push("/student/tests");
                } else {
                  history.push("/view-knowledge-spaces");
                }
              }
            })
            .catch((error) => {
              console.log(error);
            });
        }
      })
    }
    //setSubmited(true);
  }

  return <div >
    <form onSubmit={(e) => onSubmit(e)} id="login_form">
      <h2 id="login_h2">Sign In</h2>
      <p className="login_p">
        <input
          className="login_input"
          type="text"
          name="username"
          placeholder="Username"
          value={username}
          onChange={(e) => onChange(e)}
        />
        <span className="login_span" style={{ visibility: usernameAlertHidden ? 'hidden' : 'visible' }}
        >Invalid username.</span>
      </p>
      <p className="login_p">
        <input
          className="login_input"
          type="password"
          name="password"
          placeholder="Password"
          value={password}

          onChange={e => onChange(e)}
        />
        <span className="login_span" style={{ visibility: passwordAlertHidden ? 'hidden' : 'visible' }}
        >Invalid password.</span>
      </p>
      <p className="login_p">
        <input className="login_input" type="submit" value="Sign In" id="submit" />
      </p>
      <div id="route_to_register_div" >
        <h2>Don't have an account? <Link to="/register">Register</Link>  </h2>
      </div>
    </form>
  </div>;
}

export default Login;
