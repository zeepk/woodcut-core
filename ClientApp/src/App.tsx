import React from 'react';
import { BrowserRouter as Router, Switch, Route } from 'react-router-dom';
import { LandingPage } from './features/Common/LandingPage';
import { Navbar } from './features/Common/Navbar';
import './App.scss';
import 'primereact/resources/themes/arya-green/theme.css';
import 'primereact/resources/primereact.min.css';
import 'primeicons/primeicons.css';
import 'primeflex/primeflex.css';
import Rs3PlayerLandingPage from './features/RS3/Player/Rs3PlayerLandingPage';

function App() {
	return (
		<div className="App">
			<Router>
				<Navbar />
				<Switch>
					<Route exact path="/rs3/:username">
						<Rs3PlayerLandingPage />
					</Route>
					<Route path="/">
						<LandingPage />
					</Route>
				</Switch>
			</Router>
		</div>
	);
}

export default App;
