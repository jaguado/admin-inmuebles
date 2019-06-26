import http from "k6/http";
import { check, fail, sleep } from "k6";

export let options = {
	/*
		stages: [
			{
				"duration": "0m30s",
				"target": 10 
			},
			{
				"duration": "0m30s",
				"target": 100
			},
			{
				"duration": "0m30s",
				"target": 300
			},
			{
				"duration": "0m60s",
				"target": 300
			},
			{
				"duration": "0m30s",
				"target": 100
			},
			{
				"duration": "0m30s",
				"target": 10
			}
		]
	*/
	 vus: 50, duration: "60s" 
};

export default function() {
	let baseUrl = "https://admin-inmuebles-dev.herokuapp.com/";
	let requestTimeout = 0;

	//POST Retirement
	let creds= { "email": "demo", "password": "demo"};
	let params = { timeout: requestTimeout, headers: { "Content-Type": "application/json; charset=UTF-8"}};
	let login = http.post(baseUrl + "login", JSON.stringify(creds), params);	
	if(!check(login, { "Sesion iniciada": (r) => r.status === 200 }))
	{
		console.error('login.body', login.body);
		fail("ERROR AL INICIAR SESION");
	}
}
