---
layout: layout-base
order: 1
description: This is the homepage description.
documentation_items:
  - title: Overview
    description: Learn about the tools compromising Project Blue and how they work together.
    image: fa-compass
    href: <%= stache.config.blue_overview %>

  - title: Walkthroughs
    description: From setting up Visual Studio to automating product customizations, learn how to use the SDK with our 101 and 201 courses.
    image: fa-graduation-cap
    href: <%= stache.config.blue_walkthroughs %>
    
  - title: API
    description: Documentation of the available Core and Base code.  Learn what out-of-the-box functionality comes with the SDK.
    image: fa-file-text
    href: <%= stache.config.blue_api %>
---

<header class="welcome" data-stellar-background-ratio="0.5">
  <div class="text-vertical-center">
    <h1>Welcome to {{ stache.config.product_name_short }}</h1>
    <h2>An Automation SDK for <a class="welcome-header-link" href="https://www.blackbaud.com/fundraising-crm/blackbaud-nonprofit-crm">Blackbaud CRM</a>.</h2>
    <ul class="list-inline">
      <li>
        <a href="#about" class="btn btn-lg btn-primary smooth-scroll">Learn More</a>
      </li>
    </ul>
  </div>
</header>

<section id="about" class="about section-padding">
  <div class="container">
    <div class="row">
      <div class="col-sm-12 text-center">
        <h2>What is {{ stache.config.product_name_short }}?</h2>
        <p class="lead">Get an overview of what Project Blue is and how the SDK works.</p>
        <p><a href="{{stache.config.blue_overview}}" class="btn btn-lg btn-primary">Overview</a></p>
      </div>  <!-- .col-sm-12 -->
    </div>  <!-- .row -->
  </div>  <!-- .container -->
</section>  <!-- .about -->

<section id="features" class="learn section-padding bg-primary">
  <div class="container">
  
    <div class="row text-center">
    
      <div class="col-lg-10 col-lg-offset-1">
        
        <h2>{{ stache.config.product_name_short }} Walkthroughs</h2>
         <p class="lead">Our {{ stache.config.product_name_short }} walkthroughs guide you through using the SDK:</p>
         
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