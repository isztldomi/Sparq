import * as signalR from "@microsoft/signalr";

export function createConnection(path: string) {
  // console.log("SignalR Base URL:", import.meta.env.VITE_APP_SIGNALR_BASEURL);
  // console.log("Creating connection to", path);
  return new signalR.HubConnectionBuilder()
    .withUrl(`${import.meta.env.VITE_APP_SIGNALR_BASEURL}${path}`, {
      accessTokenFactory: () => localStorage.getItem("accessToken") ?? "",
    })
    .withAutomaticReconnect()
    .configureLogging(signalR.LogLevel.Information)
    .build();
}
