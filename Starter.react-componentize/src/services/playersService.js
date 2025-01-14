import axios from "axios";

const playersService = {
    endpoint: `https://localhost:50001/api/players`,
};


playersService.getTopTen = () => {
    const config = {
        method: "GET",
        url: `${playersService.endpoint}/top10`,
        crossdomain: true,
        headers: { "Content-Type": "application/json" },
    };
    return axios(config);
};

playersService.add = (payload) => {
    const config = {
        method: "POST",
        url: `${playersService.endpoint}`,
        crossdomain: true,
        data: payload,
        headers: { "Content-Type": "application/json" },
    };
    return axios(config);
};

playersService.delete = () => {
    const config = {
        method: "DELETE",
        url: `${playersService.endpoint}`,
        crossdomain: true,
        headers: { "Content-Type": "application/json" },
    };
    return axios(config);
};

playersService.downloadCsv = () => {
    const config = {
        method: "GET",
        url: `${playersService.endpoint}/downloadCsv`,
        responseType: "blob",
        crossdomain: true,
        headers: { "Content-Type": "application/json" },
    };
    return axios(config);
};


export default playersService