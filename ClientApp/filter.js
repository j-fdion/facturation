import Vue from "vue"
import moment from "moment"

Vue.filter('formatDateLLL', function(value) {
  if (value) {
      return moment.utc(String(value)).locale("fr-ca").format('LLL');
  }
})

Vue.filter('formatDateL', function (value) {
    if (value) {
        return moment.utc(String(value)).locale("fr-ca").format('LL');
    }
})

Vue.filter('nullable', function (value) {
    if (value) {
        return value;
    }
    else {
        return "";
    }
})
