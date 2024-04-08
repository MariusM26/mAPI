import axios from "axios";

const baseUrl = "http://localhost:5187/api/";

const api = {
  dCandidate(url = baseUrl + "dcandidate") {
    return {
      fetchAll: () => axios.get(url),
      fetchById: (id) => axios.get(url + "/" + id),
      create: (newRecord) => axios.post(url, newRecord),
      update: (id, updateRecord) => axios.put(url + "/" + id, updateRecord),
      delete: (id) => axios.delete(url + "/" + id),
    };
  },
};

export default api;