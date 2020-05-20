# AcornBox
Hangfire Sandbox

## Guided Tour

Using *Git Bash*,

  1. Delete your *Minikube* vm (a one node *Kubernete* cluster).
      ```
      minikube delete
      ```

  2. Create your *Minikube* vm
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

  5. Use *Minikube's Docker* daemon instead of your local (so the subsequent images are available in *Minikube*).
      ```
      eval $(minikube docker-env)
      ```

  6. Build the `acornboxwebui` *Docker* image.
      ```
      docker build -f AcornBox.WebUI/Dockerfile -t acornboxwebui:dev .
      ```

  7. Build the `acornboxwebui` *Docker* image.
      ```
      docker build -f AcornBox.Worker/Dockerfile -t acornboxworker:dev .
      ```
      
  8. Install the application into *Kubernetes*.
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
      
### Todos
  * `kubectl get deployments` scaling example / step
  * `kubectl scale deployment acornbox-acornboxworker --replicas=3` scaling example / step
  * `helm uninstall acornbox` clean up step
  * `minikube delete` clean up step
