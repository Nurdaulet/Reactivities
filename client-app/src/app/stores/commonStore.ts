import { makeAutoObservable } from "mobx";
import { ServerErrorModel } from "../models/ServerErrorModel";

export default class CommonStore {
    error: ServerErrorModel | null = null;

    constructor() {
        makeAutoObservable(this);
    }

    setServerError = (error: ServerErrorModel) => {
        this.error = error;     
    }
}