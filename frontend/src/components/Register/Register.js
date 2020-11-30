import React, { useState } from 'react';
import './Register.css';
import FormControlLabel from '@material-ui/core/FormControlLabel';
import Checkbox from '@material-ui/core/Checkbox';
import { BASE_URL, REGISTER } from '../../utils/apiUrls';
import { useHistory } from 'react-router';

function Register() {
  const history = useHistory();
  const [usernameAlertHidden, setUsernameAlertHidden] = useState(true);
  const [passwordAlertHidden, setPasswordAlertHidden] = useState(true);
  const [eMailAlertHidden, setEmailAlertHidden] = useState(true);

  const [confirmPasswordAlertHidden, setConfirmPasswordAlertHidden] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [username, setUsername] = useState('');

  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');

  const [email, setEmail] = useState('');
  const [isProfessor, setIsProfessor] = useState(false);

  const onChange = (e) => {
    if ('username' === e.target.name) {
      setUsernameAlertHidden(formValidation(e.target.value), setUsername(e.target.value));
    } else if ('password' === e.target.name) {
      setPasswordAlertHidden(formValidation(e.target.value), setPassword(e.target.value));
      validatePassword(confirmPassword);
    } else if ('confirmPassword' === e.target.name) {
      validatePassword(e.target.value);
      setConfirmPassword(e.target.value);
    } else if ('isProfessor' === e.target.name) {
      setIsProfessor(e.target.checked);
    } else if ('email' === e.target.name) {
      setEmailAlertHidden(validateEmail(e.target.value), setEmail(e.target.value));
    } else if ('firstName' === e.target.name) {
      setFirstName(e.target.value);
    } else if ('lastName' === e.target.name) {
      setLastName(e.target.value);
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

  const validateEmail = (value) => {
    if (/^[a-zA-Z0-9]+@(?:[a-zA-Z0-9]+\.)+[A-Za-z]+$/.test(value) && formValidation(value)) {
      return true;
    }
    return false;
  }
  const validateForm = () => {
    setUsernameAlertHidden(formValidation(username));
    setPasswordAlertHidden(formValidation(password));
    validatePassword(confirmPassword);
    setEmailAlertHidden(validateEmail(email));
  }

  const onSubmit = (e) => {
    e.preventDefault();
    validateForm();
    if (formValidation(username) && validatePassword(confirmPassword) && validateEmail(email)) {
      formValidation(password, () => {
        console.log('Sending register request...')
        if (usernameAlertHidden && passwordAlertHidden && (confirmPasswordAlertHidden === '')) {

          const requestOptions = {
            method: 'POST',
            headers: { 'content-type': 'application/json' },
            body: JSON.stringify({
              Username: username,
              Password: password,
              FirstName: firstName,
              LastName: lastName,
              Email: email,
              isProfessor: isProfessor
            })
          };
          const url = process.env.NODE_ENV === 'production' ? REGISTER : BASE_URL + REGISTER;
          fetch(url, requestOptions)
            .then((response) => {
              if (!response.ok) {
                alert("Invalid username or password")
              }
              else {
                alert("Successfuly registered. You may log in with your credentials");
                history.push("/login");
                return response.json();
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
    <form onSubmit={onSubmit} id="register_form">
      <h2 id="register_h2">Sign Up</h2>
      <p className="register_p">
        <input
          className="register_input"
          type="text"
          name="firstName"
          placeholder="First name"
          value={firstName}
          onChange={(e) => onChange(e)}
        />
        <span className="register_span" style={{ visibility: 'hidden'}}>needed</span>
      </p>
      <p className="register_p">
        <input
          className="register_input"
          type="text"
          name="lastName"
          placeholder="Last name"
          value={lastName}
          onChange={(e) => onChange(e)}
        />
        <span className="register_span" style={{ visibility: 'hidden'}}>needed</span>
      </p>
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
        > {confirmPasswordAlertHidden} <span style={{ visibility: 'hidden' }}>needed</span></span>
      </p>
      <p className="register_p">
        <input
          className="register_input"
          type="email"
          name="email"
          placeholder="E-mail"
          value={email}
          onChange={(e) => onChange(e)}
        />
        <span className="register_span" style={{ visibility: eMailAlertHidden ? 'hidden' : 'visible' }}
        >Enter a valid e-mail address.</span>
      </p>

      <p className="register_p">
        <FormControlLabel
          control={
            <Checkbox
              checked={isProfessor}
              onChange={onChange}
              name="isProfessor"
              color="primary"
            />
          }
          label="Register as professor"
        />
      </p>
      <p className="register_p_submit">
        <input className="register_input" type="submit" value="Create My Account" id="submit" />
      </p>
    </form>
  </div>;
}

export default Register;
