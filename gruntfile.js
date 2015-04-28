/*jslint node: true, nomen: true, plusplus: true */
'use strict';

module.exports = function(grunt) {
  
  if (!grunt.file.exists('node_modules')) {
    grunt.fail.fatal('You must run "npm install" before using Blackbaud Stache.');
  }
  
  grunt.task.loadNpmTasks('blackbaud-stache');
  grunt.task.registerTask('default', function() {
    grunt.task.run('stache');
  });
  
};