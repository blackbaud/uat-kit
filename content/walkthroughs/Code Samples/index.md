---
layout: layout-container
showHeadings: false
order: 70
name: Code Projects
description: A listing of code samples for the 101 and 202 walkthroughs.
published: true
code:
  - title: Project Blue 101 Code Sample
    description: In the following walkthroughs you'll learn to quickly create a Blackbaud CRM GUI test suite using the Blackbaud UAT SDK.
    repo: stache/
    tutorial: <%= stache.config.blue_walkthroughs %>101/

  - title: Project Blue 201 Code Sample
    description: In the following walkthroughs you'll learn how to use the UAT SDK's (Project Blue) underlying third-party tools to create custom logic and interactions with the browser.
    repo: stache-cli/
    tutorial: <%= stache.config.blue_walkthroughs %>201/
---


# Project Blue: 101 & 201 Code Samples

<div class="code">

  <div class="clearfix"></div>

  {{# eachWithMod code mod=3 }}

    {{# if firstOrMod0 }}
      <div class="row">
    {{/ if }}
        <div class="col-sm-6 col-md-4">
          <div class="thumbnail">
             {{# if itemimage }}
               <img src="{{ itemimage }}" alt="" />
            {{/ if }}
            <div class="caption">
              <h3>{{ title }}</h3>
               {{# if description }}
                <p>{{ description }}</p>
              {{/ if }}
              <a href="{{ ../stache.config.github }}{{ repo }}"  target="_blank" class="btn btn-primary" role="button">View Code</a>
              {{# if tutorial }}
                <a href="{{ tutorial }}" class="btn btn-white">
                  View Tutorial
                </a>
              {{/ if }}
            </div>
          </div>
        </div>
    {{# if lastOrMod1 }}
      </div>
    {{/ if }}

  {{/ eachWithMod }}

</div>





