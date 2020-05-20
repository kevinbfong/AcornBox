# AcornBox

*AcornBox* generates csv schemas. Upload csv files and a scalable worker will generate the files' csv schema.

## Guided Tour

Using Git Bash,

  1. Delete your Minikube vm (a one node Kubernete cluster).
      ```
      minikube delete
      ```

  2. Create your Minikube vm
      ```
      minikube start
      ```

  3. Clone the repo.
      ```
      git clone https://github.com/kevinbfong/AcornBox.git
      ```

  4. Move into the repo.
      ```
      cd AcornBox
      ```

  5. Use Minikube's Docker daemon instead of your local (so the subsequent images are available in Minikube).
      ```
      eval $(minikube docker-env)
      ```

  6. Build the `acornboxwebui` Docker image.
      ```
      docker build -f AcornBox.WebUI/Dockerfile -t acornboxwebui:dev .
      ```

  7. Build the `acornboxwebui` Docker image.
      ```
      docker build -f AcornBox.Worker/Dockerfile -t acornboxworker:dev .
      ```
      
  8. Install the application into Kubernetes.
      ```
      helm install acornbox charts/acornbox
      ```
     and then run (repeatedly, resting for a few seconds):
      ```
      kubectl get deployments
      ```
     until you see this (ignore the *Age* column):
      ```
      NAME                      READY   UP-TO-DATE   AVAILABLE   AGE
      acornbox-acornboxwebui    1/1     1            1           32m
      acornbox-acornboxworker   1/1     1            1           32m
      acornbox-mssql-linux      1/1     1            1           32m
      ```
     then the application should be ready.
                        
  9. Access the application.
      ```
      minikube service acornbox-acornboxwebui
      ```
      
  10. Explore the application. 
       1. Select *Hangfire* from the navbar. This will open the Hangfire Dashboard in a new tab. Here we can see information about our jobs.
       2. In the Hangfire Dashboard, select *Servers* from the nav bar, here we see our one worker instance. Let's adds two more instances (for a total of three), lets scale out.
       3. Back in *Git Bash*, enter `kubectl scale deployment acornbox-acornboxworker --replicas=3` and then verify with `kubectl get deployments`.
       4. Back in the Hangfire Dashboard, we should now see three instances of our worker (refresh if you don't).
       5. Back in the main application, quickly upload (one after the other) seven (7) csv files (note: the files need to have a `.csv` extension, have a `,` field delimiter, and a header record).
       6. ...
      
### Todos
  * `helm uninstall acornbox` clean up step, probably redundant.
  * `minikube delete` clean up step
