
import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.tsx'
import {Provider} from "./components/ui/provider.tsx";
import { GoogleOAuthProvider } from "@react-oauth/google";

createRoot(document.getElementById('root')!).render(
  <StrictMode>
      <GoogleOAuthProvider clientId="552250306526-65p3udigfloe4d6o7nhim817jorrlu8h.apps.googleusercontent.com">
          <Provider>
              <App />
          </Provider>
      </GoogleOAuthProvider>
  </StrictMode>,
)
