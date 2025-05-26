import { PayloadAction, Slice, SliceCaseReducers, SliceSelectors, createSlice } from '@reduxjs/toolkit'
import { IUser } from './user.interface'

interface UserState extends IUser {}
interface IUserActions {
    login: (state: UserState, action: PayloadAction<IUser>) => IUser;
    logout: (state: UserState) => void;
    renewAccessToken: (state: UserState, action: PayloadAction<string>) => void;
    renewRefreshAccessToken: (state: UserState, action: PayloadAction<string>) => void;
}

const initialState: UserState = {

    id: '',
    name: '',
    email: '',
    access_refresh_token: '',
    access_token: '',
    image: ''

}

export const userSlice = createSlice({
    name: 'user',
    initialState,
    reducers: {
        login: (state, action: PayloadAction<IUser>) => {
            console.log({
                action
            })
            return action.payload
        },

        logout: (state) => {
            // state = initialState
            return initialState
        },

        renewAccessToken: (state, action: PayloadAction<string>) => {
            // state.access_token = action.payload
            return {
                ...state,
                access_token: action.payload
            }
        },

        renewRefreshAccessToken: (state, action: PayloadAction<string>) => {
            // state.access_refresh_token = action.payload
            return {
                ...state,
                access_refresh_token: action.payload
            }
            
        }
    }
})

export const {login, logout, renewAccessToken, renewRefreshAccessToken} = userSlice.actions

export default userSlice.reducer