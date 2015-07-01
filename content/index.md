---
layout: layout-base
order: 1
description: This is the homepage description.
documentation_items:
  - title: Overview
    description: Find out how the UAT Automation Kit can help you to automate your User Acceptance Tests tests for upgrades.
    image: fa-compass
    href: <%= stache.config.blue_overview %>

  - title: Walkthroughs
    description: From setting up Visual Studio to automating product customizations, learn how to use the tool through a series of tutorials and guidelines.
    image: fa-graduation-cap
    href: <%= stache.config.blue_walkthroughs %>
    
  - title: API
    description: Refer to documentation of the available Core and Base code to see the out-of-the-box functionality that comes with the UAT Automation Kit. 
    image: fa-file-text
    href: <%= stache.config.blue_api %>
---

<header class="welcome" data-stellar-background-ratio="0.5">
  <div class="text-vertical-center">
    <h1>Welcome to the {{ stache.config.product_name_long }}</h1>
    <h1>Early Adopter Program</h1>
    <h2><strong>This website is for the Early Adopter program<br>for Blackbaud's automation tool for <strong><em><a class="welcome-header-link" href="https://www.blackbaud.com/fundraising-crm/blackbaud-nonprofit-crm">Blackbaud CRM</a></em></strong>.<br>It is not intended for general use at this point,<br>and the website and its documentation<br> are subject to change.</strong></h2>
    <ul class="list-inline">
      <li>
        <a href="{{stache.config.blue_walkthroughs_getting-started}}" class="btn btn-lg btn-primary">Get Started</a>
      </li>
    </ul>
  </div>
</header>

<section id="about" class="about section-padding">
  <div class="container">
    <div class="row">
      <div class="col-sm-12 text-center">
        <h2>Use the {{ stache.config.product_name_short }} to Create a Suite of Automated Tests</h2>
        <p class="lead">The automation tool is a set of tools, templates, samples, and documentation to enhance and accelerate the automation of User Acceptance Testing for Blackbaud CRM and other Infinity applications.</p>
<!--        <p><a href="{{ stache.config.tutorials_getting_started }}" class="btn btn-lg btn-primary">Getting Started</a></p>-->
      </div>  <!-- .col-sm-12 -->
    </div>  <!-- .row -->
  </div>  <!-- .container -->
</section>  <!-- .about -->


<section id="features" class="learn section-padding bg-primary">
  <div class="container">
  
    <div class="row text-center">
    
      <div class="col-lg-10 col-lg-offset-1">
        
        <h2>{{ stache.config.product_name_short }} Resources</h2>
         
        {{# eachWithMod documentation_items mod=3 }}

            {{# if firstOrMod0 }}
        <div class="row col-lg-offset-0"> 
            {{/ if }}
          
          <div class="col-md-4 col-sm-6">
            <a href="{{ href }}" class="btn-fa-link">
              <span class="fa-stack fa-4x">
                <i class="fa fa-square fa-stack-2x"></i>{{# if image }}
                <i class="fa {{ image }} fa-stack-1x text-primary"></i>{{/ if }}
              </span>  <!-- .fa-stack -->
            </a>  <!-- .btn-fa-link -->
            <h4><strong>{{ title }}</strong></h4>
            {{# if description }}
            <p>{{ description }}</p>
            {{/ if }}
          </div>  <!-- .col-md-3 -->
          
          
            {{# if lastOrMod1 }}
        </div>  <!-- .row -->
            {{/ if }}
        {{/ eachWithMod }}
      </div>  <!-- .col-lg-10 -->
    </div>  <!-- .row -->
  </div>  <!-- .container -->
</section>  <!-- .learn -->

<!--
<section id="start" class="start section-padding">
  <div class="container">
    <div class="row">
      <div class="col-sm-12">
        <h2>Get Help</h2>
        <p>Got questions?  We want to help! 
          Check out <a href="{{ stache.config.stache_docs_resources_faq }}">frequently asked questions</a></p>
      </div>  
    </div>  
  </div>  
</section>
-->