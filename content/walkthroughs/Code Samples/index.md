---
layout: layout-container
showHeadings: false
order: 70
name: Code Projects
description: A listing of code samples for the 101 and 202 walkthroughs.
published: true
code:
  - title: Project Blue 101 Code Sample
    description: This is the application code that goes with our Web API tutoroial.  It includes an example of using the Authorization Code flow.
    repo: stache/
    tutorial: <%= stache.config.base %>

  - title: Project Blue 201 Code Sample
    description: A more specific example of the constituent api where we search based on different criteria.
    repo: stache-cli/
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





