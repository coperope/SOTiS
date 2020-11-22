import React, { useState } from 'react';
import './Register.css';

function Register() {
  const [usernameAlertHidden, setUsernameAlertHidden] = useState(true);
  const [passwordAlertHidden, setPasswordAlertHidden] = useState(true);
  const [confirmPasswordAlertHidden, setConfirmPasswordAlertHidden] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [username, setUsername] = useState('');

  const onChange = (e) => {
    if ('username' === e.target.name) {
      setUsernameAlertHidden(formValidation(e.target.value), setUsername(e.target.value));
    } else if ('password' === e.target.name) {
      setPasswordAlertHidden(formValidation(e.target.value), setPassword(e.target.value));
      validatePassword(confirmPassword);
    } else if ('confirmPassword' === e.target.name) {
      validatePassword(e.target.value);
      setConfirmPassword(e.target.value);
    }
  }
  const validatePassword = (confirmedPass) => {
    if (!formValidation(confirmedPass)) {
      setConfirmPasswordAlertHidden("Enter a password longer than 3 characters");
    } else if (confirmedPass !== password) {
      setConfirmPasswordAlertHidden("Your passwords do not match");
    } else {
      setConfirmPasswordAlertHidden('');
    }
    return confirmPasswordAlertHidden === '' ? true : false;
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
    if (formValidation(username) && validatePassword(confirmPassword)) {
      formValidation(password, () => {
        console.log('sending request...')
        if (usernameAlertHidden && passwordAlertHidden && (confirmPasswordAlertHidden === '')) {
          /* Todo: uncomment and correct when backend available.
          const requestOptions = {
            method: 'POST',
            headers: { 'content-type': 'application/json' },
            body: JSON.stringify({
              username: username,
              password: password
            })
          };
          const url = process.env.NODE_ENV === 'production' ? "rest/user/login" : "http://localhost:8080/ChatWar/rest/chat/users/login";

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
                this.props.setLoggedInUser(data);
                this.props.history.push('/chat');
              }
            })
            .catch((error) => {
              console.log(error);
            });*/
        }
      })
    }
    //setSubmited(true);
  }

  return <div >
    <form onSubmit={onSubmit} id="register_form">
      <h2 id="register_h2">Sign Up</h2>
      <p className="register_p">
        <input
          className="register_input"
          type="text"
          name="username"
          placeholder="Username"
          value={username}
          onChange={(e) => onChange(e)}
        />
        <span className="register_span" style={{ visibility: usernameAlertHidden ? 'hidden' : 'visible' }}
        >Enter a username longer than 3 characters</span>
      </p>
      <p className="register_p">
        <input
          className="register_input"
          type="password"
          name="password"
          placeholder="Password"
          value={password}

          onChange={(e) => onChange(e)}
        />
        <span className="register_span" style={{ visibility: passwordAlertHidden ? 'hidden' : 'visible' }}
        >Enter a password longer than 3 characters</span>
      </p>
      <p className="register_p">
        <input
          className="register_input"
          type="password"
          name="confirmPassword"
          placeholder="Confirm password"
          value={confirmPassword}

          onChange={(e) => onChange(e)}
        />
        <span className="register_span" style={{ visibility: confirmPasswordAlertHidden === '' ? 'hidden' : 'visible' }}
        >{ confirmPasswordAlertHidden }</span>
      </p>
      <p className="register_p">
        <input className="register_input" type="submit" value="Create My Account" id="submit" />
      </p>
    </form>
  </div>;
}

export default Register;
