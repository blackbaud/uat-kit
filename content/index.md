---
layout: layout-base
order: 1
description: This is the homepage description.
documentation_items:
  - title: Overview
    description: Learn about the tools compromising the automation SDK and how they work together.
    image: fa-compass
    href: <%= stache.config.blue_overview %>

  - title: Walkthroughs
    description: From setting up Visual Studio to automating product customizations, learn how to use the SDK with our tutorials.
    image: fa-graduation-cap
    href: <%= stache.config.blue_walkthroughs %>
    
  - title: API
    description: Documentation of the available Core and Base code. Learn what out-of-the-box functionality comes with the SDK.
    image: fa-file-text
    href: <%= stache.config.blue_api %>
---

<header class="welcome" data-stellar-background-ratio="0.5">
  <div class="text-vertical-center">
    <h1>Welcome to the {{ stache.config.product_name_long }}</h1>
    <h2>An automation tool for <a class="welcome-header-link" href="https://www.blackbaud.com/fundraising-crm/blackbaud-nonprofit-crm">Blackbaud CRM</a>.</h2>
    <ul class="list-inline">
      <li>
        <a href="{{stache.config.blue_walkthroughs_getting-started}}" class="btn btn-lg btn-primary">Get Started</a>
      </li>
    </ul>
  </div>
</header>

<p class="alert alert-warning"><strong><em>Warning:</em></strong> This website is for the early adopter program for the {{ stache.config.product_name_long }}. It is not intended for general use at this point, and the documentation is in a preliminary state and is subject to change.</p>

<section id="about" class="about section-padding">
  <div class="container">
    <div class="row">
      <div class="col-sm-12 text-center">
        <h2>What is the {{ stache.config.product_name_long }}?</h2>
        <p class="lead">Get an overview of the automation tool and how to use it to create a suite of automated tests.</p>
        <p><a href="{{stache.config.blue_overview}}" class="btn btn-lg btn-primary">Overview</a></p>
      </div>  <!-- .col-sm-12 -->
    </div>  <!-- .row -->
  </div>  <!-- .container -->
</section>  <!-- .about -->

<section id="features" class="learn section-padding bg-primary">
  <div class="container">
  
    <div class="row text-center">
    
      <div class="col-lg-10 col-lg-offset-1">
        
        <h2>{{ stache.config.product_name_short }} Resources</h2>
         <p class="lead">Our walkthroughs get you up and running and guide you through using the {{ stache.config.product_name_short }}.</p>
         
        {{# eachWithMod documentation_items mod=3 }}

            {{# if firstOrMod0 }}
        <div class="row col-lg-offset-0"> 
            {{/ if }}
          
          <div class="col-md-4 col-sm-6">
            <a href="{{ href }}" class="btn-fa-link">
              <span class="fa-stack fa-4x">
                <i class="fa fa-square fa-stack-2x"></i>
                {{# if image }}
                <i class="fa {{ image }} fa-stack-1x text-primary"></i>
                {{/ if }}
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