import Vue from 'vue'

import router from './router'
import store from './store'
import { sync } from 'vuex-router-sync'
import App from 'components/app-root'

import Vuetify from 'vuetify';
import './stylus/main.styl';

Vue.use(Vuetify);

import datePicker from 'vue-bootstrap-datetimepicker';
import 'eonasdan-bootstrap-datetimepicker/build/css/bootstrap-datetimepicker.css';
Vue.use(datePicker);

import 'moment';
import axios from 'axios';
import 'fullcalendar';
import "fullcalendar-scheduler";
import "fullcalendar/dist/fullcalendar.min.css";
import "fullcalendar-scheduler/dist/scheduler.min.css";

import "./filter"

//Vue.prototype.$http = axios;
sync(store, router)

const app = new Vue(
    {
        store,
        router,
        ...App
    });

export {
    app,
    router,
    store
}
