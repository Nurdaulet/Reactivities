import { makeAutoObservable, reaction } from "mobx";
import { ServerErrorModel } from "../models/ServerErrorModel";

export default class CommonStore {
    error: ServerErrorModel | null = null;
    token: string | null = window.localStorage.getItem('jwt');
    appLoaded = false;

    constructor() {
        makeAutoObservable(this);

        reaction(
            () => this.token,
            token => {
                if (token) {
                    window.localStorage.setItem('jwt', token);
                } else {
                    window.localStorage.removeItem('jwt');
                }
            }
        )
    }

    setServerError = (error: ServerErrorModel) => {
        this.error = error;
    }

    setToken = (token: string | null) => {
        if (token) window.localStorage.setItem('jwt', token);
        this.token = token;
    }

    setAppLoaded = () => {
        this.appLoaded = true;
    }
}