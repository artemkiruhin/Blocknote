import config from "./api-handler-config.json";

const BASE_URL = config["server-url"];
const API_URL = `${BASE_URL}/api`;

export {
    BASE_URL,
    API_URL,
};