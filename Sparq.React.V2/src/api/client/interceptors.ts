import { apiClient } from "./apiClient";

export function setupInterceptors() {
  apiClient.interceptors.request.use((config) => {
    const auth = localStorage.getItem("auth");

    //console.log("AUTH:", auth);

    if (auth) {
      const parsed = JSON.parse(auth);

      config.headers.Authorization = `Bearer ${parsed.token}`;
    }

    //console.log("HEADERS:", config.headers);

    return config;
  });

  apiClient.interceptors.response.use(
    (res) => res,
    (err) => {
      return Promise.reject(err);
    },
  );
}

//import { apiClient } from "./apiClient";
//
//export function setupInterceptors() {
//  apiClient.interceptors.request.use((config) => {
//    const user = localStorage.getItem("user");
//    const auth = localStorage.getItem("auth");
//    console.log("AUTH:", auth);
//    if (user) {
//      const parsed = JSON.parse(user);
//      config.headers.Authorization = `Bearer ${parsed.authToken}`;
//    }
//    console.log("HEADERS:", config.headers);
//    return config;
//  });
//
//  apiClient.interceptors.response.use(
//    (res) => res,
//    (err) => {
//      // ide jön a ProblemDetails logika
//      return Promise.reject(err);
//    },
//  );
//}
