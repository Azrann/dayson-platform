module.exports = function(grunt) {
  grunt.initConfig({
    connect: {
      dev: {
        options: {
          port: 8080,
          keepalive: true,
          base: '.',
          directory: '.',
          open: {
            target: 'http://localhost:8080/login.html'
          }
        }
      },
      production: {
        options: {
          port: 8080,
          keepalive: true,
          base: '../production',
          directory: '../production',
          open: {
            target: 'http://localhost:8080/login.html'
          }
        },
      }
    },
    ngtemplates:{
      'login-app':{
        module: 'login-app',
        standalone: false,
        cwd: '.',
        src:['app/login-app/views/**/*.html'],
        dest:'../build/js/templates/login-app-tpl.js'
      }
    },
    clean: {
      options: { force: true },
      files: ['../build', '../production']
    },
    concat: {
      options: {
        separator: ';\n',
        nonull: true,
      },
      'js-libs': {
        src: [
          'bower_components/jquery/dist/jquery.js',
          'bower_components/angular/angular.js',
          'bower_components/angular-route/angular-route.js',
          'bower_components/angular-messages/angular-messages.js',
          'bower_components/angular-translate/angular-translate.js',
          'bower_components/angular-ui/build/angular-ui.js',
        ],
        dest: '../build/js/dayson.lib.js'
      },
      'dayson-lib': {
        src: [
          'app/dayson-common/dayson-common.js',
          'app/dayson-common/services/authService.js',
          'app/dayson-common/directives/equals.js',
        ],
        dest: '../build/js/dayson.common.js'
      },
      'login-app': {
        src: [
          '../build/js/dayson.common.js',
          'app/login-app/loginApp.js',
          'app/login-app/services/authService.js',
          'app/login-app/views/forgot/forgot.js',
          'app/login-app/views/forgot/forgotController.js',
          'app/login-app/views/login/login.js',
          'app/login-app/views/login/loginController.js',
          'app/login-app/views/register/register.js',
          'app/login-app/views/register/registerController.js',
          'app/login-app/views/resetPassword/resetPassword.js',
          'app/login-app/views/resetPassword/resetPasswordController.js',
          '../build/js/templates/login-app-tpl.js',
        ],
        dest: '../build/js/dayson.login.js'
      },
      'css-libs': {
        options: {
          separator: '',
        },
        src: [
          'bower_components/bootstrap/dist/css/bootstrap.css',
          'bower_components/bootstrap/dist/css/bootstrap-theme.css',
        ],
        dest: '../build/css/dayson.lib.css'
      },
      'dayson-login-css': {
        options: {
          separator: '',
        },
        src: [
          'css/dayson.css',
          'css/dayson.login.css',
        ],
        dest: '../build/css/dayson.login.css'
      }
    },
    uglify: {
      options: {
        report: 'min',
      },
      production: {
        files: {
          '../production/js/dayson.lib.min.js': '../build/js/dayson.lib.js',
          '../production/js/dayson.login.min.js': '../build/js/dayson.login.js',
        }
      }
    },
    cssmin: {
      production: {
        files: {
          '../production/css/dayson.lib.min.css': '../build/css/dayson.lib.css',
          '../production/css/dayson.login.min.css': '../build/css/dayson.login.css',
        }
      }
    },
    copy: {
      fonts: {
        nonull: true,
        expand: true,
        cwd: 'bower_components/bootstrap/dist/fonts/',
        src: ['*'],
        dest: '../production/fonts'
      }
    },
    processhtml: {
      options: {
        process: true,
      },
      production: {
        files: {
          '../production/login.html': 'login.html',
        }
      }
    },
    ftp_push: {
      production: {
        options: {
          username: "REPLACE_WITH_YOUR_VALUE",
          password: "REPLACE_WITH_YOUR_VALUE",
          host: "ftp.gear.host",
          dest: "/site/wwwroot",
          port: 21
        },
        files: [
          {
            expand: true,
            cwd: '../production/',
            src: [
              "**"
            ]
          }
        ]
      }
    }
  });

  grunt.loadNpmTasks('grunt-angular-templates');
  grunt.loadNpmTasks('grunt-contrib-clean');
  grunt.loadNpmTasks('grunt-contrib-concat');
  grunt.loadNpmTasks('grunt-contrib-connect');
  grunt.loadNpmTasks('grunt-contrib-copy');
  grunt.loadNpmTasks('grunt-contrib-cssmin');
  grunt.loadNpmTasks('grunt-contrib-uglify');
  grunt.loadNpmTasks('grunt-contrib-watch');
  grunt.loadNpmTasks('grunt-processhtml');
  grunt.loadNpmTasks('grunt-processhtml');
  grunt.loadNpmTasks('grunt-ftp-push');

  grunt.registerTask('web', ['connect:dev']);
  grunt.registerTask('clear', ['clean']);
  grunt.registerTask('build', ['clear', 'ngtemplates', 'concat', 'uglify', 'cssmin', 'processhtml', 'copy']);
  grunt.registerTask('production', ['build', 'connect:production']);
  grunt.registerTask('deploy', ['build', 'ftp_push:production']);

  grunt.registerTask('default', ['web']);
};
