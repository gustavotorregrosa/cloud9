import { configureStore } from '@reduxjs/toolkit'
import userReducer from './user/user.slice'
import { IUser } from './user/user.interface'

export interface IState {
    user: IUser
}

export const makeStore = () => configureStore({
    reducer: {
        user: userReducer
    },
})

export type AppStore = ReturnType<typeof makeStore>