<template>
    <v-app>
        <template v-if="authenticated">
            <v-navigation-drawer fixed v-model="drawer" app>
                <v-list>
                    <v-list-tile v-for="route in routes">
                        <template v-if="route.draw">
                            <v-list-tile-content>
                                <v-list-tile-title>
                                    <router-link :to="route.path">
                                        <span :class="route.style"></span> {{ route.display }}
                                    </router-link>
                                </v-list-tile-title>
                            </v-list-tile-content>
                        </template>
                    </v-list-tile>
                </v-list>
            </v-navigation-drawer>
        </template>
        <v-toolbar color="grey darken-4" dark fixed app>
            <v-toolbar-side-icon @click.stop="drawer = !drawer"></v-toolbar-side-icon>
            <v-toolbar-title>Groupe Conseil Filion</v-toolbar-title>
            <v-spacer></v-spacer>
            <v-toolbar-items class="hidden-sm-and-down">
                <template v-if="authenticated">
                    <v-btn flat v-on:click='logout'>Déconnexion</v-btn>
                </template>
                <template v-else>
                    <v-btn flat v-on:click='$auth.loginRedirect'>Connexion</v-btn>
                </template>
            </v-toolbar-items>
        </v-toolbar>
        <v-content>
            <v-container fluid>
                <router-view>
                </router-view>
            </v-container>
        </v-content>
        <v-footer color="grey darken-4" app>
            <span class="white--text">&copy; 2018</span>
        </v-footer>
    </v-app>
</template>

<script>

    import Vue from 'vue'
    import { routes } from '../routes'

    import HomePage from './home-page'
    import FullCalendar from './FullCalendar'
    import PersonneForm from './personne/personneForm'
    import Personne from './personne/personne'
    import EntrepriseForm from './entreprise/entrepriseForm'
    import Entreprise from './entreprise/entreprise'
    import FormateurForm from './formateur/formateurForm'
    import Formateur from './formateur/formateur'
    import TypeAccessoireForm from './typeAccessoire/typeAccessoireForm'
    import TypeAccessoire from './typeAccessoire/typeAccessoire'
    import AccessoireForm from './accessoire/accessoireForm'
    import Accessoire from './accessoire/accessoire'
    import FormationForm from './formation/formationForm'
    import Formation from './formation/formation'
    import ContactForm from './contact/contactForm'
    import Contact from './contact/contact'
    import FraisSupplementaireForm from './fraisSupplementaire/fraisSupplementaireForm'
    import FraisSupplementaire from './fraisSupplementaire/fraisSupplementaire'
    import SalleForm from './salle/salleForm'
    import Salle from './salle/salle'
    import SessionForm from './session/sessionForm'
    import Session from './session/session'
    import Participant from './participant/participant'
    import ParticipantForm from './participant/participantForm'
    import FraisSupplementaireSession from './fraisSupplementaireSession/fraisSupplementaireSession'
    import FraisSupplementaireSessionForm from './fraisSupplementaireSession/fraisSupplementaireSessionForm'
    import FactureForm from './facture/factureForm'
    

    Vue.component('full-calendar', FullCalendar);
    Vue.component('home-page', HomePage);
    Vue.component('personne-form', PersonneForm);
    Vue.component('entreprise-form', EntrepriseForm);
    Vue.component('formateur-form', FormateurForm);
    Vue.component('typeAccessoire-form', TypeAccessoireForm);
    Vue.component('accessoire-form', AccessoireForm);
    Vue.component('formation-form', FormationForm);
    Vue.component('contact-form', ContactForm);
    Vue.component('fraisSupplementaire-form', FraisSupplementaireForm);
    Vue.component('contact-form', ContactForm);
    Vue.component('salle-form', SalleForm);
    Vue.component('session-form', SessionForm);
    Vue.component('participant', Participant);
    Vue.component('participant-form', ParticipantForm);
    Vue.component('fraisSupplementaireSession', FraisSupplementaireSession);
    Vue.component('fraisSupplementaireSession-form', FraisSupplementaireSessionForm);
    Vue.component('facture-form', FactureForm);

    import axios from 'axios'
    import Auth from '@okta/okta-vue'
  
    axios.interceptors.request.use(function (config) {
        Vue.prototype.$auth.getAccessToken().then(token => {
                config.headers['Authorization'] = 'Bearer ' + token
            }
        )
        return config;

    }.bind(Vue));

export default {
        data: function () {
            return {
                routes,
                drawer: true,
                authenticated: false,
                userInfo: null
            }
        },
          created () {
            this.checkAuthentication()
        },
        watch: {
            // Every time the route changes, check the auth status
            '$route': 'checkAuthentication'
        },
        methods: {
            async checkAuthentication() {
                let previouslyLoggedIn = this.authenticated
                this.authenticated = await this.$auth.isAuthenticated()
                let justLoggedIn = !previouslyLoggedIn && this.authenticated
                if (justLoggedIn) {
                    this.userInfo = await this.$auth.getUser()
                }
                let justLoggedOut = previouslyLoggedIn && !this.authenticated
                if (justLoggedOut) {
                    this.userInfo = null
                }
            },
            async logout() {
                await this.$auth.logout()
                await this.checkAuthentication()
                // Navigate back to home
                this.$router.push({ path: '/' })
            }
        }

}
</script>

<style>
</style>
