import config from "./api-handler-config.json"

const BASE_URL = config["server-base-url"];
const API_URL = `${config["server-base-url"]}/api`;

export {
    BASE_URL,
    API_URL,
}