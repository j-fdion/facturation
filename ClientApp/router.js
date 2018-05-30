import Vue from 'vue'
import VueRouter from 'vue-router'
import Auth from '@okta/okta-vue'

import { routes } from './routes'

Vue.use(VueRouter);

Vue.use(Auth, {
    // Replace this with your Okta domain:
    issuer: 'https://dev-100319.oktapreview.com/oauth2/default',
    // Replace this with the client ID of the Okta app you just created:
    client_id: 'cle',
    //redirect_uri: 'appurl',
    redirect_uri: 'https://localhost:44321/implicit/callback',
    scope: 'openid profile email'
})

let router = new VueRouter({
    mode: 'history',
    routes
})

// Check the authentication status before router transitions
router.beforeEach(Vue.prototype.$auth.authRedirectGuard())

export default router
