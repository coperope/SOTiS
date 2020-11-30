
import './App.css';
import React from 'react';
import { HashRouter as Router, Route } from 'react-router-dom'

import Login from './components/Login/Login.js'
import ListTests from './components/ListTests/ListTests.tsx'
import Register from './components/Register/Register.js'
import Header from './components/Header/Header.js'
import TestView from './components/TestView/TestView'

function App() {
  return (
    <Router basename="/">
      <Header></Header>
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
        <Route exact path="/student/tests" render={props => (
          <React.Fragment>
            <ListTests></ListTests>
          </React.Fragment>
        )} />
        <Route exact path="/student/test/:testId" render={props => (
          <React.Fragment>
            <TestView></TestView>
          </React.Fragment>
        )} />
      </div>
    </Router>
  );
}

export default App;
