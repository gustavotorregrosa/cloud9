import { IState } from "@/store";
import { IUser } from "@/store/user/user.interface";
import { login, logout, renewAccessToken, renewRefreshAccessToken } from "@/store/user/user.slice";
import { EnhancedStore } from "@reduxjs/toolkit";
import { toast } from "react-toastify";

export class ConnectionService {

    constructor(public store: EnhancedStore<IState>) {}

    generateHeaders = (refresh: boolean = false) => {

        if(!this.store.getState().user.access_token && !this.store.getState().user.access_refresh_token){
             const userFromLocalStorage = localStorage.getItem('user')
             if(userFromLocalStorage){
                const user = JSON.parse(userFromLocalStorage) as IUser
                this.store.dispatch(login(user))
             }
        }

        return {
            Authorization: `Bearer ${refresh ? this.store.getState().user.access_refresh_token : this.store.getState().user.access_token}`,
            "Content-type": "application/json; charset=UTF-8",
        }
    }

    makeSingleRequest = async <T>(endpoint: string, method: 'get' | 'post' | 'put' | 'patch' | 'delete', body?: string) => {
        return await fetch(process.env.NEXT_PUBLIC_API_URL + '/' + endpoint, {
            headers: this.generateHeaders(),
            method: method.toUpperCase(),
            body,
            
        })
    }

    makeRequest = async <T>(endpoint: string, method: 'get' | 'post' | 'put' | 'patch' | 'delete', body?: string) => {
        let response = await this.makeSingleRequest(endpoint, method, body)
        if(response.status == 401){
            await this.updateUser()
            response = await this.makeSingleRequest(endpoint, method, body)
        }
        
        return await response.json() as T
    }

    updateUser = async () => {
        const response = await fetch(process.env.NEXT_PUBLIC_API_URL + '/authentication/refreshtoken', { headers: this.generateHeaders(true) })
        if(response.status == 401){
            const msg = 'Refresh token expired'
            toast.error(msg, {
                toastId: msg
            })
            this.store.dispatch(logout())
            return
        }
        const userToken: IUser = await response.json()
        userToken && this.store.dispatch(login(userToken))
    }

}