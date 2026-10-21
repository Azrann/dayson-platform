angular.module('login-app').controller('loginController', [
  'authService',
  function (authService) {
    this.submitted = false;

    this.executeLogin = function () {
      this.submitted = true;
      this.errors = {};
      this.errors.wrongUserPassword = true;

      authService.login('email','password');
    }
  }]);
