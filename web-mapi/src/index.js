import React from 'react';
import { createRoot } from 'react-dom/client';
// import ReactDOM from 'react-dom';
import { Provider } from 'react-redux';
import {store} from '../src/actions/store.js'; // Import your Redux store
import App from './App';
import './index.css'

// ReactDOM.render(
//   <Provider store={store}>
//     <App />
//   </Provider>,
//   document.getElementById('root')
// );

createRoot(document.getElementById('root')).render(
  <Provider store={store}>
    <App />
  </Provider>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
