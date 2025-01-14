import React from "react";
import "./App.css";
import PlayerList from "./components/players/PlayersList";

function App() {
  return (
    <div className="app-container">
      <h1 className="app-title">PSH exercise</h1>
      <PlayerList></PlayerList>
    </div>
  );
}

export default App;
