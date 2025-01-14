import React, { useEffect, useState } from "react";
import PlayerCard from "./PlayerCard";
import playersService from "../../services/playersService";
import "./playerlist.css";
import toastr from "toastr";

function PlayerList() {
  const [topPlayers, setTopPlayers] = useState({
    originalData: [],
    mappedData: [],
  });
  const [dateGenerated, setDateGenerated] = useState(null);
  const [show, setShow] = useState(false);

  useEffect(() => {
    const fetchData = () => {
      const onGetTopTenSuccess = (response) => {
        let arrayOfPlayers = response.data.items;
        console.log("arrayOfPlayers", arrayOfPlayers);

        setTopPlayers((prevState) => {
          let newState = { ...prevState };
          newState.originalData = arrayOfPlayers;
          newState.mappedData = arrayOfPlayers.map(mappingPlayersList);
          return newState;
        });

        // Update the generated time
        setDateGenerated(response.data.dateGenerated);
      };

      const onGetTopTenError = (response) => {
        console.error(response);
        setTopPlayers((prevState) => {
          let newState = { ...prevState };
          newState.originalData = [];
          newState.mappedData = [];
          return newState;
        });
      };

      playersService
        .getTopTen()
        .then(onGetTopTenSuccess)
        .catch(onGetTopTenError);
    };

    fetchData();
    const intervalId = setInterval(fetchData, 10000);

    return () => clearInterval(intervalId);
  }, []);

  const mappingPlayersList = (aPlayer) => {
    return <PlayerCard key={aPlayer.id} player={aPlayer}></PlayerCard>;
  };

  const onShowDev = () => {
    setShow((prevState) => !prevState);
  };

  const formatDateTime = (dateString) => {
    const options = {
      year: "numeric",
      month: "long",
      day: "numeric",
      hour: "2-digit",
      minute: "2-digit",
      second: "2-digit",
    };
    return new Date(dateString)
      .toLocaleString(undefined, options)
      .replace(/ AM| PM/g, "");
  };

  const onExportCsv = () => {
    playersService
      .downloadCsv()
      .then((response) => {
        const url = window.URL.createObjectURL(new Blob([response.data]));
        const link = document.createElement("a");
        link.href = url;
        link.setAttribute("download", "players.csv");
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
      })
      .catch((error) => {
        console.error("Error downloading CSV:", error);
        toastr.error("Failed to download CSV file.");
      });
  };

  const onAddWinner = () => {
    let winner = {
      statId: 6,
      NickName: "Mr. Winner",
      ProfileImage:
        "https://i.natgeofe.com/n/548467d8-c5f1-4551-9f58-6817a8d2c45e/NationalGeographic_2572187_2x3.jpg",
      Score: 101,
    };

    playersService.add(winner).then(onAddSuccess).catch(onAddError);
  };

  const onAddSuccess = (response) => {
    console.log(response);
    toastr.success("add success", "Winner Added");
  };

  const onAddError = (response) => {
    console.error(response);
    toastr.error("add success", "Winner NOT added");
  };

  const onClearDB = () => {
    playersService.delete().then(onDeleteSuccess).catch(onDeleteError);
  };

  const onDeleteSuccess = (response) => {
    console.log(response);
    toastr.success("delete success", "database Clear");
  };

  const onDeleteError = (response) => {
    console.error(response);
    toastr.error("delete error", "check logs");
  };

  return (
    <React.Fragment>
      <div className="player-list-container">
        <div className="dev-cheats-row">
          <button className="btn btn-danger" onClick={onShowDev}>
            Dev Cheats
          </button>
          <button className="btn btn-secondary ms-3" onClick={onExportCsv}>
            Export CSV
          </button>
          <span className="stats-generated">
            Stats generated on:{" "}
            {dateGenerated ? formatDateTime(dateGenerated) : "Loading..."}
          </span>
        </div>

        <h1 className="player-list-title">Ranking</h1>
        <br />
        <br />
        {show && (
          <>
            <button className="btn btn-success" onClick={onAddWinner}>
              Add Winner
            </button>
            <br />
            <br />
            <button className="btn btn-warning" onClick={onClearDB}>
              Clear Db
            </button>
          </>
        )}

        <div className="container-player-cards">{topPlayers.mappedData}</div>
      </div>
    </React.Fragment>
  );
}

export default PlayerList;
