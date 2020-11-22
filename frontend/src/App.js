
import './App.css';
import React from 'react';
import { HashRouter as Router, Route } from 'react-router-dom'
import Login from './components/Login/Login.js'
import Register from './components/Register/Register.js'

function App() {
  return (
    <Router basename="/">
      <div className="App">
        <Route exact path="/" render={props => (
          <React.Fragment>
            <Login></Login>
          </React.Fragment>
        )} />
        <Route exact path="/login" render={props => (
          <React.Fragment>
            <Login></Login>
          </React.Fragment>
        )} />
        <Route exact path="/register" render={props => (
          <React.Fragment>
            <Register></Register>
          </React.Fragment>
        )} />
      </div>
    </Router>
  );
}

export default App;
