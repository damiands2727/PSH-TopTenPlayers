import React from "react";
import PropTypes from "prop-types";
import "./playercard.css";

function PlayerCard({ player }) {
  const statColor = player.statName.split(" ")[0].toLowerCase();

  return (
    <div className="player-card-horizontal d-flex align-items-center p-2">
      <img
        src={player.profileImage}
        alt={player.nickName}
        className="player-card-image me-3"
        style={{ borderColor: statColor }}
      />
      <div className="player-card-info d-flex w-100">
        <div className="player-card-column player-card-name-column">
          <h2 className="player-card-name">{player.nickName}</h2>
        </div>
        <div className="player-card-column player-card-score-column text-center">
          <p className="player-card-score">
            Score:{" "}
            <span className="player-card-score-value">{player.score}</span>
          </p>
        </div>
        <div className="player-card-column player-card-date-column text-center">
          <p className="player-card-date">
            Joined:{" "}
            <span className="player-card-score-value">
              {new Date(player.dateCreated).toLocaleDateString()}
            </span>
          </p>
        </div>
        <div className="player-card-column player-card-stat-column text-center">
          <p className="player-card-stat">
            Stat:{" "}
            <span className="player-card-stat-name">{player.statName}</span>
          </p>
        </div>
      </div>
    </div>
  );
}

PlayerCard.propTypes = {
  player: PropTypes.shape({
    id: PropTypes.number.isRequired,
    nickName: PropTypes.string.isRequired,
    profileImage: PropTypes.string.isRequired,
    dateCreated: PropTypes.string.isRequired,
    score: PropTypes.number.isRequired,
    statId: PropTypes.number.isRequired,
    statName: PropTypes.string.isRequired,
  }).isRequired,
};

export default PlayerCard;
