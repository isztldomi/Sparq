import * as signalR from "@microsoft/signalr";

export function createHubConnection(path: string) {
  return new signalR.HubConnectionBuilder()
    .withUrl(`${import.meta.env.VITE_APP_SIGNALR_BASEURL}${path}`, {
      accessTokenFactory: () => localStorage.getItem("accessToken") ?? "",
    })
    .withAutomaticReconnect()
    .configureLogging(signalR.LogLevel.Information)
    .build();
}
