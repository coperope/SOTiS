
import './App.css';
import React from 'react';
import { HashRouter as Router, Route } from 'react-router-dom'

import Login from './components/Login/Login.js'
import ListTests from './components/ListTests/ListTests.tsx'
import CreateTest from './components/CreateTest/CreateTest.tsx'

import Register from './components/Register/Register.js'
import Header from './components/Header/Header.js'
import TestView from './components/TestView/TestView'
import { CreateKnowledgeSpace } from './components/CreateKnowledgeSpace/CreateKnowledgeSpace'
import ListKnowledgeSpaces from './components/ListKnowledgeSpaces/ListKnowledgeSpaces'

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
        <Route exact path="/tests" render={props => (
          <React.Fragment>
            <ListTests></ListTests>
          </React.Fragment>
        )} />
        <Route exact path="/student/test/:testId" render={props => (
          <React.Fragment>
            <TestView></TestView>
          </React.Fragment>
        )} />
        <Route exact path="/createTest" render={props => (
          <React.Fragment>
            <CreateTest></CreateTest>
          </React.Fragment>
        )} />
        <Route exact path="/knowledge-space" render={props => (
          <React.Fragment>
            <CreateKnowledgeSpace></CreateKnowledgeSpace>
          </React.Fragment>
        )} />
        <Route exact path="/knowledge-space/:id" render={props => (
          <React.Fragment>
            <CreateKnowledgeSpace></CreateKnowledgeSpace>
          </React.Fragment>
        )} />
        <Route exact path="/view-knowledge-spaces" render={props => (
          <React.Fragment>
            <ListKnowledgeSpaces></ListKnowledgeSpaces>
          </React.Fragment>
        )} />
      </div>
    </Router>
  );
}

export default App;
